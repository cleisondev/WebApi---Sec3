using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using WebApi___Sec3.Context;
using WebApi___Sec3.DTOs.Mappings;
using WebApi___Sec3.Extensions;
using WebApi___Sec3.Filters;
using WebApi___Sec3.Logging;
using WebApi___Sec3.Models;
using WebApi___Sec3.Repositories;
using WebApi___Sec3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();




builder.Services.AddCors(options =>
    options.AddPolicy("OrigensComAcessoPermitido",
    policy =>
    {
        policy.WithOrigins("http://localhost:xxxx")
        .WithMethods("GET", "POST")
        .AllowAnyHeader();
    })
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apicatalogo", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
    AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


//builder.Services.AddAuthorization();    
//builder.Services.AddAuthentication("Bearer").AddJwtBearer();

string sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");


var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Invalid Secret key");//Primeiro obter a chave secreta

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Esquema de desafio
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//Esquema de autenticação
}).AddJwtBearer(options =>
{
    options.SaveToken = true; //Configurando o Jwt
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,//O que será usado na validação
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "cleison"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));

    options.AddPolicy("ExclusivePolicyOnly", policy =>
        policy.RequireAssertion(context =>
        context.User.HasClaim(claim =>
                            claim.Type == "id" && claim.Value == "cleison")
                            || context.User.IsInRole("SuperAdmin")));
});

builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(sqlConnection));



builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter(policyName: "fixedWindow", options =>
    {
        options.PermitLimit = 1; //Uma requisicação
        options.Window = TimeSpan.FromSeconds(5); // A cada cinco segundos
        options.QueueLimit = 2; //Nao vai enfileirar
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

//Configurando taxa global
builder.Services.AddRateLimiter(options =>
{
    //Usando limite particionado, baseado no contexto http
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext =>
                            RateLimitPartition.GetFixedWindowLimiter(
                                                partitionKey: httpcontext.User.Identity?.Name ??//Tenta usar o usuario ou o host como usauri de contexto
                                                              httpcontext.Request.Headers.Host.ToString(),
                                                factory: partition => new FixedWindowRateLimiterOptions
                                                {
                                                    AutoReplenishment = true,
                                                    PermitLimit = 5,
                                                    QueueLimit = 0,
                                                    Window = TimeSpan.FromSeconds(10)
                                                }));
});


builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);//Versão padrão
    o.AssumeDefaultVersionWhenUnspecified = true;//Quando nao especificar a versao, usar a padrao
    o.ReportApiVersions = true;//Versao da api vai no header do response
    o.ApiVersionReader = ApiVersionReader.Combine(
                            new QueryStringApiVersionReader(),
                            new UrlSegmentApiVersionReader());
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";//Formato na documentação
    options.SubstituteApiVersionInUrl = true;//Substiuir versao da api na url ao gerar links pra diferentes versoes da api
});



builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfig
{
    LogLevel = LogLevel.Information,
}));

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseRateLimiter();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

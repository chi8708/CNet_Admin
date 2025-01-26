using Autofac;
using Autofac.Extensions.DependencyInjection;
using CNet.Main.BLL;
using CNet.Common;
using CNet.Main.DAL;
using CNet.Main.Model;
using CNet.Web.Api.Controllers;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using CNet.Web.Api.Model;
using AutoMapper.Internal;
using System.Text.RegularExpressions;
using static NPOI.HSSF.Util.HSSFColor;
using Microsoft.AspNetCore.StaticFiles;
using System.Xml.Schema;
using System.ComponentModel.Design;
using Microsoft.Extensions.Options;
using CNet.Web.Api.Config;
using CNet.Common.Const;

namespace CNet.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            LogConfig();

        }

        //日志配置
        private static void LogConfig()
        {
            // //log4Net
            var logTypes = Enum.GetValues(typeof(LogType));
            foreach (LogType logType in logTypes)
            {
                var repository = LogManager.CreateRepository(logType.ToString());
                XmlConfigurator.Configure(repository, new FileInfo(Environment.CurrentDirectory + "/log4net.config"));
            }
        }

        public IConfiguration Configuration { get; }
        /// <summary>
        /// 这段代码必须放在Startup类里面
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {

            //构造函数注入
            //builder.RegisterType<IBaseDataDapperContrib>().As<BaseDataDapperContrib>();
            //属性注入：
            //builder.RegisterType<EmployeeService>().As<IEmployeeService>().PropertiesAutowired();//只能在当前的EmployeeService类，使用属性注入
            //Autofac批量
            //需要 using 命名空间 System.Reflection  Straup.cs 文件中的 ConfigureContainer() 方法 
            //约定接口（Interface）和实现（class）都是以 Service 【或者其他】结尾的。
            //泛型注册

            //builder.RegisterGeneric(typeof(BaseDataDapperContrib<>)).As(typeof(IBaseDataDapperContrib<>));
            //builder.RegisterGeneric(typeof(BaseServiceDapperContrib<>)).AsSelf().PropertiesAutowired();//属性注册服务//假如要注册就用从控制器开始注册
            ////var basedir = Path.Combine(Directory.GetCurrentDirectory(), "lib");

            ////var assemblysBLL = Assembly.Load($"{basedir}/CNet.BLL.dll");//Service是继承接口的实现方法类库名称
            ////var assemblysDAL = Assembly.LoadFile($"{basedir}/CNet.DAL.dll");//Service是继承接口的实现方法类库名称
            //var assemblysBLL = Assembly.Load($"CNet.BLL");
            //var assemblysDAL = Assembly.Load($"CNet.DAL");
            //////var baseType = typeof(IBaseService<>);//IDependency 是一个接口（所有要实现依赖注入的借口都要继承该接口）

            //builder.RegisterAssemblyTypes(assemblysBLL)
            //     .Where(t => (t.BaseType != null && t.BaseType.Name.StartsWith("BaseService")))
            //     .AsSelf().InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(assemblysDAL)
            //     .Where(t => (t.BaseType != null && t.BaseType.Name.StartsWith("BaseDataDapperContrib")))
            //    .AsSelf().InstancePerLifetimeScope();

        }



        //依赖注入服务
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //1.全局异常 2.Json 日期格式化
            services
                .AddMvc(o =>
                {
                    o.Filters.Add(typeof(WebApiExceptionAttribute));
                    //Swagger剔除不需要加入api展示的列表
                    o.Conventions.Add(new ApiExplorerIgnores());
                    o.EnableEndpointRouting = false;
                });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //参考 https://www.cnblogs.com/aishangyipiyema/p/9262642.html
            JWTConfig(services);

            SwaggerConfig(services);

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {

                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });

            });
            //services.AddControllers();

            services.AddControllers(options =>
            {
                //options.Filters.Add(typeof(ApiExceptionFilter));
            });

            //同步读取body的方式需要ConfigureServices中配置允许同步读取IO流，否则可能会抛出异常 Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                        .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            services.AddMemoryCache();//内存缓存
            Service_DI(services);

            //添加session支持(session依赖于cache进行存储)
            services.AddSession();

        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services"></param>
        private static void Service_DI(IServiceCollection services)
        {
            services.AddScoped<DI_Test>();//依赖注入
            //var basedir = Path.Combine(Directory.GetCurrentDirectory(), "lib");
            //var basedir = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            Assembly? rootAssembly = Assembly.GetEntryAssembly();
            if (rootAssembly == null)
            {
                rootAssembly = Assembly.GetCallingAssembly();
            }
            var assembiles = new List<Assembly>();
            var assembilesName = rootAssembly.GetReferencedAssemblies().Where(p => p.Name.StartsWith("CNet")).ToList();
            assembilesName.ForEach(name => { assembiles.Add(Assembly.Load(name)); });

            //批量依赖注入
            //var assembiles = Zack.Commons.ReflectionHelper.GetAllReferencedAssemblies();
            //assembiles = assembiles.Where(p => p.FullName.StartsWith("CNet"));
            //services.RunModuleInitializers(assembiles);
        }

        private static void SwaggerConfig(IServiceCollection services)
        {
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CNet API",
                    Version = "v1",
                    Description = "CNet基础框架API",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                //不加AddSecurityRequirement请求头不会有authorization
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {
                    new OpenApiSecurityScheme
                    {
                      Reference=new OpenApiReference
                      {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                      }
                    },
                    new string[] {}
                  }
                });
                //swagger中控制请求的时候发是否需要在url中增加accesstoken
                // c.OperationFilter<AuthTokenHeaderParameter>();

                // 为 Swagger JSON and UI设置xml文档注释路径
                //HttpContext.Current.Request.PhysicalApplicationPath
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "CNet.Web.Api.xml");
                c.IncludeXmlComments(xmlPath, true);

                var xmlPathModel = Path.Combine(AppContext.BaseDirectory, "CNet.Model.xml");
                c.IncludeXmlComments(xmlPathModel, true);

            });
        }


        /// <summary>
        /// 使用 Microsoft.AspNetCore.Authentication.JwtBearer
        /// </summary>
        /// <param name="services"></param>
        private void JWTConfig(IServiceCollection services)
        {
            services.Configure<JwtSeetings>(Configuration.GetSection("JwtSeetings"));
            var jwtSeetings = new JwtSeetings();
            //绑定jwtSeetings
            Configuration.Bind("JwtSeetings", jwtSeetings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSeetings.Issuer,
                    ValidAudience = jwtSeetings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSeetings.SecretKey)),
                    ValidateLifetime = false //验证生命周期
                };
            });

        }

        //中间件
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            
            if (!env.IsDevelopment())
            {
                app.UseSwaggerAuthorizedMildd();
            }

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CNet.Web.Api v1");
                    //设置默认跳转到swagger-ui
                 c.RoutePrefix = AppSettingsConst.SwaggerRoutePrefix;
            });

            //Microsfot.Extensions.Logging.Log4Net.AspNetCore 需添加
            //loggerFactory.AddLog4Net(Environment.CurrentDirectory + "//log4net.config");
            //app.UseHttpsRedirection();//会跳转跨域时不要使用


            app.UseRouting();
            // app.UseMvc();

            //app.UseStaticFiles();
            var basePath = System.IO.Directory.GetCurrentDirectory();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(basePath + "//FileUpload//"),
                RequestPath = "/FileUpload"
            });

            ////jwt认证 需要在app.UseMvc()前调用
            app.UseAuthentication();//不添加报401
            app.UseCors("default");
            // app.UseCors(anyAllowSpecificOrigins);//支持跨域：允许特定来源的主机访问

            app.UseAuthorization();
            //app.UseResponseCaching();//服务端缓存。客户端禁用缓存后也能缓存。确保 app.UseResponseCaching(); 在app.UseCors("default")后， app.MapControllers前。


            //request.body的长度总是为0
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllers().RequireCors("default");
            });

        }
    }
}

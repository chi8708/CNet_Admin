//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseAuthorization();

//app.MapControllers();

//app.Run();



var builder = WebApplication.CreateBuilder(args);

// ��� MVC ����
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // ���� Razor ����ʱ����

var app = builder.Build();

// ���� HTTP ����ܵ�
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ���þ�̬�ļ�֧�֣��� CSS��JS �ȣ�

app.UseRouting();

app.UseAuthorization();

// ���� MVC ·��
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
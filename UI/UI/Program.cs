var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.
builder.Services.AddControllers(); // Add support for API controllers
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(); // For Blazor Server apps

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Map API controllers
app.MapControllers();
app.MapHub<MessageHub>("/messageHub");
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

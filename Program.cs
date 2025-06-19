using DBconnection;
using Firebase.Storage;
using Microsoft.AspNetCore.Http.HttpResults;
using Reaq;
using skeleton;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
new DBconnection.DB();

app.MapPost("/CadPost", async (HttpContext content) =>
{
    using var reader = new StreamReader(content.Request.Body);
    var body = await reader.ReadToEndAsync();
    var info = JsonSerializer.Deserialize<skeleton.Posts>(body);

    new Reaq.Posts(info.desc, info.Image, info.PostID, info.User, info.likes, info.date);

    var returned = await Reaq.Posts.PostMessage();

    return Results.Ok(returned);
});

app.MapGet("/consultPost", async (HttpContext content) =>
{
    using var reader = new StreamReader(content.Request.Body);
    var body = await reader.ReadToEndAsync();
    var info = JsonSerializer.Deserialize<skeleton.Posts>(body);

    var posts = new Reaq.Posts(info.desc, info.Image, info.PostID, info.User, info.likes, info.date);

    var returned = await posts.ConsultMessage();
    System.Console.WriteLine("this is returned: " + JsonSerializer.Serialize(returned));

    return Results.Ok(returned);
});
app.MapGet("/consultAll", async () =>
{
    var posts = new Reaq.Posts(null, null, 0, null, 0, null);

    var returned = await posts.ConsultAll();
    System.Console.WriteLine("this is returned: " + JsonSerializer.Serialize(returned));

    return Results.Ok(returned);
});








app.MapPost("/cadUser", async (HttpContext content) =>
{
    Console.WriteLine("this is env: " + Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"));

    using var reader = new StreamReader(content.Request.Body);
    var body = await reader.ReadToEndAsync();
    var info = JsonSerializer.Deserialize<skeleton.user>(body);
    var result = await Users.CadUser(info);

    return Results.Ok(result);
});











app.MapPost("/follow", async (HttpContext content) =>
{
    using var reader = new StreamReader(content.Request.Body);
    var body = await reader.ReadToEndAsync();
    var info = JsonSerializer.Deserialize<skeleton.Followers>(body);

    System.Console.WriteLine("this is info: " + info.followedID);
    var Test = new Reaq.Follows(info.followedID, info.followerID);
    await Test.Follow();
    return Results.Ok();
});
app.MapDelete("/unfollow", async (HttpContext content)=>
{
    using var reader = new StreamReader(content.Request.Body);
    var body = await reader.ReadToEndAsync();
    var info = JsonSerializer.Deserialize<skeleton.Followers>(body);
    
    System.Console.WriteLine("this is info: " + info.followedID);
    var Test = new Reaq.Follows(info.followedID, info.followerID);
    
    await Test.unfollow();
    return Results.Ok("unfollowed");
});


app.Run();

﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(EmployeeRequest employeeRequest, HttpContext httpContext, UserManager<IdentityUser> userManager)
    {
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var newUser = new IdentityUser
        {
            UserName = employeeRequest.Email,
            Email = employeeRequest.Email,
        };

        var result = await userManager.CreateAsync(newUser, employeeRequest.Password);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.Name),
            new Claim("CreatedBy", userId)
        };

        var claimResult = await userManager.AddClaimsAsync(newUser, userClaims);

        if (!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors.First());

        return Results.Created($"/employees/{newUser.Id}", newUser.Id);
    }
}

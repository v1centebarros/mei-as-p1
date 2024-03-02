﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace api.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
    }
}
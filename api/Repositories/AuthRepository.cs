using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using api.Models;
using api.Models.DTOs;
using api.Models.Contracts;
using api.Data;
using static api.Models.DTOs.ServiceResponses;

namespace api.Repositories
{
    public class AuthRepository : IUserAuth
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthRepository(UserManager<ApplicationUser> userManager, AppDbContext context, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<GeneralResponse> Create(UserDTO userDTO)
        {
            if (userDTO == null) return new GeneralResponse(false, "Model is empty");

            if (await _userManager.FindByEmailAsync(userDTO.Email) != null) 
                return new GeneralResponse(false, "User already registered");

            var user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.Email, PhoneNumber = userDTO.PhoneNumber };
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded) return new GeneralResponse(false, "Error creating user");

            var patient = new Patient
            {
                ApplicationUser = user,
                FullName = userDTO.FullName,
                MedicalRecord = new MedicalRecord
                {
                    MedicalRecordNumber = Guid.NewGuid().ToString(),
                    TreatmentPlan = userDTO.TreatmentPlan,
                    DiagnosisDetails = userDTO.DiagnosisDetails,
                    AccessCode = Guid.NewGuid().ToString(),
                }
            };

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            await AssignRole(user);

            return new GeneralResponse(true, "Account created successfully");
        }

        private async Task AssignRole(ApplicationUser user)
        {
            var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var userRole = adminRoleExists ? "User" : "Admin";
            if (userRole == "User" && !await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, userRole);
        }

        public async Task<LoginResponse> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null) return new LoginResponse(false, null, "Login container is empty");

            var user = await _context.Patients
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(x => x.ApplicationUser.Email == loginDTO.Email);

            if (user == null) return new LoginResponse(false, null, "User not found");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user.ApplicationUser, loginDTO.Password);
            if (!isPasswordValid) return new LoginResponse(false, null, "Invalid email/password");

            var roles = await _userManager.GetRolesAsync(user.ApplicationUser);
            var token = GenerateToken(user, roles.FirstOrDefault());

            return new LoginResponse(true, token, "Login completed successfully");
        }

        private string GenerateToken(Patient user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim(ClaimTypes.Email, user.ApplicationUser.Email),
                new Claim(ClaimTypes.Role, role ?? "User"), // Default to "User" if role is null
            };

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}

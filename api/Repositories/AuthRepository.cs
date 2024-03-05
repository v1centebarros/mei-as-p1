using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Data;
using api.Models.Contracts;
using api.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static api.Models.DTOs.ServiceResponses;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IUserAuth
    {

        public async Task<GeneralResponse> Create(UserDTO userDTO)
        {
            if (userDTO is null) return new GeneralResponse(false, "Model is empty");

            var medicalRecord = new MedicalRecord
            {
                MedicalRecordNumber = Guid.NewGuid().ToString(),
                TreatmentPlan = userDTO.TreatmentPlan,
                DiagnosisDetails = userDTO.DiagnosisDetails,
                AccessCode = Guid.NewGuid().ToString(),
            };

            var newUser = new ApplicationUser()
            {
                FullName = userDTO.FullName,
                Email = userDTO.Email,
                UserName = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
                MedicalRecord = medicalRecord
            };
            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return new GeneralResponse(false, "User registered already");

            var createUser = await userManager.CreateAsync(newUser!, userDTO.Password);
            if (!createUser.Succeeded) return new GeneralResponse(false, "Error occured.. please try again");

            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(true, "Account Created");
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await userManager.AddToRoleAsync(newUser, "User");
                return new GeneralResponse(true, "Account Created");
            }
        }

        public async Task<LoginResponse> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponse(false, null!, "Login container is empty");

            //Get User and the Medical Record
            var getUser = await userManager.Users.Include(x => x.MedicalRecord).FirstOrDefaultAsync(x => x.Email == loginDTO.Email);
            if (getUser is null)
                return new LoginResponse(false, null!, "User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkUserPasswords)
                return new LoginResponse(false, null!, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);


            // Check if MedicalRecord is null
            if (getUser.MedicalRecord == null)
            {
                // Handle the case where MedicalRecord is not set
                // For example, you might want to return an error response
                return new LoginResponse(false, null!, "Medical record not found");
            }


            var userSession = new UserSession(getUser.Id, getUser.FullName, getUser.Email, getUser.MedicalRecord.TreatmentPlan, getUser.MedicalRecord.MedicalRecordNumber, getUser.MedicalRecord.AccessCode, getUser.MedicalRecord.DiagnosisDetails, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, "Login completed");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using LiquourApp.Data;
using LiquourApp.Models;
using LiquourApp.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LiquourApp.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponse> Login(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };
            }

            if (!VerifyPasswordHash(model.Password, user.PasswordHash))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Contraseña incorrecta"
                };
            }

            // Actualizar último inicio de sesión
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generar token JWT
            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Success = true,
                Token = token.token,
                Expiration = token.expiration,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<AuthResponse> Register(RegisterModel model)
        {
            // Verificar si el usuario ya existe
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "El correo electrónico ya está registrado"
                };
            }

            // Verificar la edad (debe ser mayor de 18 años)
            if (!IsAdult(model.DateOfBirth))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Debes ser mayor de 18 años para registrarte"
                };
            }

            // Crear nuevo usuario
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                DateOfBirth = model.DateOfBirth,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generar token JWT
            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Success = true,
                Message = "Registro exitoso",
                Token = token.token,
                Expiration = token.expiration,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public bool IsAdult(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            // Ajustar la edad si aún no ha llegado el cumpleaños este año
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= 18;
        }

        private string HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Combinar salt y hash en un solo string para almacenar
            var hashBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

            return Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashBytes = Convert.FromBase64String(storedHash);

            // Extraer salt (primeros 64 bytes)
            var salt = new byte[64];
            Array.Copy(hashBytes, 0, salt, 0, 64);

            // Calcular hash con la contraseña proporcionada y el salt almacenado
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Comparar el hash calculado con el hash almacenado
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != hashBytes[64 + i])
                {
                    return false;
                }
            }

            return true;
        }

        private (string token, DateTime expiration) GenerateJwtToken(User user)
    {
        // Generar una clave única para este usuario y sesión
        string uniqueKey = GenerateUniqueKey(user);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(uniqueKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName),
            new Claim("TokenGenerationTime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))
        };

        // Fijo a 60 minutos por requerimiento
        var expiration = DateTime.UtcNow.AddMinutes(60);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }
    
    private string GenerateUniqueKey(User user)
    {
        // Combinar la clave base con información del usuario y tiempo actual
        string baseKey = _configuration["Jwt:Key"];
        string userSpecificData = $"{user.Id}-{user.Email}-{DateTime.UtcNow.ToString("yyyy-MM-dd-HH")}";
        
        // Usar un hash para generar una clave única pero determinista para la hora actual
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(baseKey + userSpecificData));
            return Convert.ToBase64String(hashedBytes);
        }
    }
    }
}
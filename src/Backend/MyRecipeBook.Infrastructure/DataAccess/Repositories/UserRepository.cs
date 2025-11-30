
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository ,IUserReadOnlyRepository
{
    private readonly MyrecipeBookDbContext _dbContext;

    public UserRepository(MyrecipeBookDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.users.AnyAsync(user => user.Email.Equals(email) && user.Active);


}
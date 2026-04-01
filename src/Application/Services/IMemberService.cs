using Domain.Entities;

namespace Application.Services;

public interface IMemberService
{
    Task<Member> AddAsync(Member member);
    Task<Member> UpdateAsync(Member member);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Pull_Projects
{
    internal class Atrybuty
    {
        public async Task UpdateVerificationsForProcessAsync(VerificationsForProcessDto dto)
        {
            // Usuń wszystkie istniejące powiązania
            var existing = await _context.VerificationsForProcess
                .Where(x => x.ProcessTypeId == dto.Id)
                .ToListAsync();

            _context.VerificationsForProcess.RemoveRange(existing);

            // Dodaj nowe powiązania z dto
            var newLinks = dto.Verifications.Select(v => new VerificationsForProcess
            {
                ProcessTypeId = dto.Id,
                VerificationTypeId = v.Id
            });

            await _context.VerificationsForProcess.AddRangeAsync(newLinks);

            await _context.SaveChangesAsync();
        }

        public async Task AddVerificationsForProcessAsync(string processId, List<string> verificationIds)
        {
            var existing = await _context.VerificationsForProcess
                .Where(x => x.ProcessTypeId == processId)
                .Select(x => x.VerificationTypeId)
                .ToListAsync();

            var toAdd = verificationIds
                .Except(existing)
                .Select(vid => new VerificationsForProcess
                {
                    ProcessTypeId = processId,
                    VerificationTypeId = vid
                });

            await _context.VerificationsForProcess.AddRangeAsync(toAdd);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveVerificationsFromProcessAsync(string processId, List<string> verificationIdsToRemove)
        {
            var toRemove = await _context.VerificationsForProcess
                .Where(x => x.ProcessTypeId == processId && verificationIdsToRemove.Contains(x.VerificationTypeId))
                .ToListAsync();

            _context.VerificationsForProcess.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<VerificationsForProcessDto> GetVerificationsForProcessAsync(string processId)
        {
            var process = await _context.ProcessTypes
                .Include(p => p.VerificationsForProcess)
                    .ThenInclude(vp => vp.VerificationType)
                .FirstOrDefaultAsync(p => p.Id == processId);

            return new VerificationsForProcessDto
            {
                Id = process.Id,
                Description = process.Description,
                Verifications = process.VerificationsForProcess
                    .Select(vp => new VerificationDto
                    {
                        Id = vp.VerificationTypeId
                    })
                    .ToList()
            };
        }
    }
}

private AppDbContext GetInMemoryDbContext()
{
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // każdorazowo nowa baza
        .Options;

    return new AppDbContext(options);
}

[Fact]
public async Task UpdateVerificationsForProcess_ShouldReplaceExistingVerifications()
{
    // Arrange
    var context = GetInMemoryDbContext();

    var process = new ProcessType { Id = "P1", Description = "Process A" };
    var v1 = new VerificationType { Id = "V1", Description = "Check 1" };
    var v2 = new VerificationType { Id = "V2", Description = "Check 2" };
    var v3 = new VerificationType { Id = "V3", Description = "Check 3" };

    await context.ProcessTypes.AddAsync(process);
    await context.VerificationTypes.AddRangeAsync(v1, v2, v3);
    await context.VerificationsForProcess.AddAsync(new VerificationsForProcess
    {
        ProcessTypeId = "P1",
        VerificationTypeId = "V1"
    });
    await context.SaveChangesAsync();

    var service = new VerificationService(context);

    // Act
    await service.UpdateVerificationsForProcessAsync(new VerificationsForProcessDto
    {
        Id = "P1",
        Description = "Process A",
        Verifications = new List<VerificationDto>
            {
                new VerificationDto { Id = "V2" },
                new VerificationDto { Id = "V3" }
            }
    });

    var updated = await context.VerificationsForProcess
        .Where(vp => vp.ProcessTypeId == "P1")
        .ToListAsync();

    // Assert
    Assert.Equal(2, updated.Count);
    Assert.Contains(updated, x => x.VerificationTypeId == "V2");
    Assert.Contains(updated, x => x.VerificationTypeId == "V3");
}

[Fact]
public async Task GetVerifications_ShouldReturnCorrectDtos()
{
    // Arrange
    var context = GetInMemoryDbContext();

    var process = new ProcessType { Id = "P1", Description = "Proc" };
    var v1 = new VerificationType { Id = "V1", Description = "Check 1" };
    var v2 = new VerificationType { Id = "V2", Description = "Check 2" };

    await context.ProcessTypes.AddAsync(process);
    await context.VerificationTypes.AddRangeAsync(v1, v2);
    await context.VerificationsForProcess.AddRangeAsync(
        new VerificationsForProcess { ProcessTypeId = "P1", VerificationTypeId = "V1" },
        new VerificationsForProcess { ProcessTypeId = "P1", VerificationTypeId = "V2" }
    );
    await context.SaveChangesAsync();

    var service = new VerificationService(context);

    // Act
    var dto = await service.GetVerificationsAsync("P1");

    // Assert
    Assert.Equal("P1", dto.Id);
    Assert.Equal(2, dto.Verifications.Count);
    Assert.Contains(dto.Verifications, v => v.Id == "V1");
    Assert.Contains(dto.Verifications, v => v.Id == "V2");
}

modelBuilder.Entity<VerificationsForProcess>()
      .HasKey(vp => new { vp.ProcessTypeId, vp.VerificationTypeId });

modelBuilder.Entity<VerificationsForProcess>()
    .HasOne(vp => vp.ProcessType)
    .WithMany(p => p.VerificationsForProcess)
    .HasForeignKey(vp => vp.ProcessTypeId);

modelBuilder.Entity<VerificationsForProcess>()
    .HasOne(vp => vp.VerificationType)
    .WithMany(v => v.VerificationsForProcess) // jeśli dodasz nawigację również po stronie VerificationType
    .HasForeignKey(vp => vp.VerificationTypeId);

modelBuilder.Entity<ProcessType>()
    .HasMany(p => p.VerificationTypes)
    .WithMany(v => v.ProcessTypes)
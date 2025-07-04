﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pull_Projects
{
    internal class newAtrybuty
    {
        public async Task AddProcessAsync(ProcessDto dto)
        {
            var verifications = await _context.VerificationTypes
                .Where(v => dto.VerificationIds.Contains(v.Id))
                .ToListAsync();

            var process = new ProcessType
            {
                Id = dto.Id,
                Description = dto.Description,
                VerificationTypes = verifications
            };

            _context.ProcessTypes.Add(process);
            await _context.SaveChangesAsync();
        }

        await _context.ProcessTypes
       .Include(p => p.VerificationTypes)


        .private FirstOrDefaultAsync(p => p.Id == dto.Id);

    if (process == null)

        throw private new Exception("Process not found");

        process.Description = dto.Description;

    // Usuń stare powiązania
    process.VerificationTypes.Clear();

    // Dodaj nowe
    private var newVerifications = await _context.VerificationTypes
        .Where(v => dto.VerificationIds.Contains(v.Id))
        .ToListAsync();

    foreach (var v in newVerifications)
        process.VerificationTypes.Add(v);

    await _context.SaveChangesAsync();
    }

    public async Task<ProcessDto> GetProcessAsync(string processId)
        {
            var process = await _context.ProcessTypes
                .Include(p => p.VerificationTypes)
                .FirstOrDefaultAsync(p => p.Id == processId);

            if (process == null)
                return null;

            return new ProcessDto
            {
                Id = process.Id,
                Description = process.Description,
                VerificationIds = process.VerificationTypes.Select(v => v.Id).ToList()
            };
        }

        public async Task DeleteProcessAsync(string processId)
        {
            var process = await _context.ProcessTypes
                .Include(p => p.VerificationTypes)
                .FirstOrDefaultAsync(p => p.Id == processId);

            if (process == null)
                return;

            // Odłącz weryfikacje (nie usuwa ich z bazy)
            process.VerificationTypes.Clear();

            _context.ProcessTypes.Remove(process);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProcessDto>> GetAllProcessesAsync()
        {
            return await _context.ProcessTypes
                .Include(p => p.VerificationTypes)
                .Select(p => new ProcessDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    VerificationIds = p.VerificationTypes.Select(v => v.Id).ToList()
                })
                .ToListAsync();
        }
    }
}
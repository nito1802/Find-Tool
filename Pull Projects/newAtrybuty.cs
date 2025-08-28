using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pull_Projects
{
    /*

     SELECT
    VerificationTimeInMinutes,
    CASE
        WHEN VerificationTimeInMinutes IS NOT NULL THEN
        ELSE CONCAT(
            VerificationTimeInMinutes / 1440, 'd ',
            (VerificationTimeInMinutes % 1440) / 60, 'g ',
            VerificationTimeInMinutes % 60, 'min'
        )
    END AS DniGodzMin
FROM [nito_SomeApps].[HandleryTest].[JobHandlers];

     */

    /*

     using System.Net.Http.Headers;

var client = new HttpClient();
var token = "abc123"; // wygenerowany token SonarQube
var byteArray = System.Text.Encoding.ASCII.GetBytes($"{token}:");
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bdasic", Convert.ToBase64String(byteArray));

var url = "http://localhost:9000/api/measures/component?component=my-project&metricKeys=coverage,bugs";
var response = await client.GetStringAsync(url);

Console.WriteLine(response);

    <span style="color:white; font-size:10px;">Ukryty biały mały tekst</span>

    {code:title=Przykład w Code Macro|borderStyle=solid|borderColor=#ccc|borderWidth=1px|bgColor=#f0f0f0|color=red}
Ten tekst będzie czerwony w bloku kodu.
{code}

   {panel:bgColor=red|borderStyle=solid|borderColor=red|title=Kolorowy panel}
Ten tekst będzie w czerwonym panelu.
{panel}

    {color:red}Ten tekst będzie czerwony.{color}

     */

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
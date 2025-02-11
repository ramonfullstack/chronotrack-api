using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace ChronotrackService.Application
{
    public class ExtraHourService : IExtraHourService
    {
        private readonly ChronotrackContext chronotrackContext;

        public ExtraHourService(
            ChronotrackContext chronotrackContext)
        {
            this.chronotrackContext = chronotrackContext;
        }

        public async Task<List<ExtraHoursEntity>> GetExtraHoursByUserAsync(int idUser)
        {
            return await chronotrackContext.ExtraHours
                .Where(e => e.IdUser == idUser)
                .ToListAsync();
        }

        public async Task<ExtraHoursEntity> SaveOrUpdateExtraHourAsync(ExtraHoursEntity extraHour)
        {
            if (extraHour.Id == 0)
            {
                extraHour.Created = DateTime.UtcNow;
                extraHour.Updated = DateTime.UtcNow;
                await chronotrackContext.ExtraHours.AddAsync(extraHour);
            }
            else
            {
                chronotrackContext.ExtraHours.Update(extraHour);
            }

            await chronotrackContext.SaveChangesAsync();
            return extraHour;
        }

        public async Task<ExtraHoursEntity> GetExtraHourByIdAsync(int id)
        {
            return await chronotrackContext.ExtraHours
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<byte[]> GenerateCsvReportAsync(int idUser)
        {
            var extraHours = await chronotrackContext.ExtraHours
                .Where(e => e.IdUser == idUser)
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExtraHours");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "ID do Usuário";
                worksheet.Cell(1, 3).Value = "Dia da Semana";
                worksheet.Cell(1, 4).Value = "Horas Trabalhadas";
                worksheet.Cell(1, 5).Value = "Valor Total do Dia";
                worksheet.Cell(1, 6).Value = "Data de Criação";

                for (int i = 0; i < extraHours.Count; i++)
                {
                    var extraHour = extraHours[i];

                    worksheet.Cell(i + 2, 1).Value = extraHour.Id;
                    worksheet.Cell(i + 2, 2).Value = extraHour.IdUser;
                    worksheet.Cell(i + 2, 3).Value = extraHour.DayOfWeek;
                    worksheet.Cell(i + 2, 4).Value = extraHour.HoursWorked;
                    worksheet.Cell(i + 2, 5).Value = extraHour.TotalValueEarnedDay;
                    worksheet.Cell(i + 2, 6).Value = extraHour.Created.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // Salvar como CSV em um MemoryStream
                using (var stream = new MemoryStream())
                {
                    // Salva a planilha como CSV
                    workbook.SaveAs(stream);

                    // Retorna o conteúdo CSV como byte array
                    return stream.ToArray();
                }
            }

        }
    }
}

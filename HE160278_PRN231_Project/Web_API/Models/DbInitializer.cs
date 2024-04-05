namespace Web_API.Models
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDBContext context)
        {
            if(!context.Subjects.Any()) {
                context.Subjects.AddRange(new List<Subject>
                {
                    new Subject { Code = "EXE101", Name = "Khởi nghiệp 1"},
                    new Subject { Code = "EXE201", Name = "Khởi nghiệp 2"},
                    new Subject { Code = "MLN111", Name = "Trính trị Mác Lê-nin"},
                    new Subject { Code = "MLN122", Name = "Kinh tế chính trị Mác Lê-nin"},
                    new Subject { Code = "PRN211", Name = ".NET 1"},
                    new Subject { Code = "PRN221", Name = ".NET 2"},
                    new Subject { Code = "PRN231", Name = ".NET 3"},
                });
                context.SaveChanges();
            }

        }
    }
}

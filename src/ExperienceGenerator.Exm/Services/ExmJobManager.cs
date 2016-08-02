namespace ExperienceGenerator.Exm.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using ExperienceGenerator.Exm.Models;
  using Sitecore.Jobs;

  public class ExmJobManager
    {
        public static ExmJobManager Instance;

        private readonly List<ExmDataPreparationModel> _jobs = new List<ExmDataPreparationModel>();

        public ExmJob StartJob(ExmDataPreparationModel specification)
        {
            specification.Job = new ExmJob();
            this._jobs.Add(specification);
            var options = new JobOptions("ExperienceGeneratorExm-" + specification.Job.Id, "ExperienceGenerator", Sitecore.Context.Site.Name, this, "Run", new object[] { specification });
            JobManager.Start(options);
            return specification.Job;
        }

        public void Run(ExmDataPreparationModel specification)
        {
            var exmDataPreparationService = new ExmDataPreparationService(specification);
            exmDataPreparationService.CreateData();
        }

        public ExmJob Poll(Guid id)
        {
            var spec = this._jobs.FirstOrDefault(x => x.Job.Id == id);
            return spec == null ? null : spec.Job;
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeciBackend.Data
{
    public class QueJobs
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long job_id { get; set; }
        public byte priority { get; set; }
        public DateTimeOffset run_at { get; set; }
        public string job_class { get; set; }
        public string args { get; set; }
        public int error_count { get; set; }
        public string last_error { get; set; }
        public string queue { get; set; }
    }
}
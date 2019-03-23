﻿using System;
using TaskManager.DAL.Models;

namespace TaskManager.DTO.Task
{
    public class TaskItemDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long ElapsedTime { get; set; }

        public DateTime LastStartTime { get; set; }

        public TimeSpan? Goal { get; set; }

        public bool IsRunning { get; set; }

        public int UserId { get; set; }

        public WatchType WatchType { get; set; }
    }
}
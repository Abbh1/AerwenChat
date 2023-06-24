using Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.System;

namespace ARW.Tasks
{
	public interface ITaskSchedulerServer
	{
		Task<ApiResult> StartTaskScheduleAsync();

		Task<ApiResult> StopTaskScheduleAsync();

		Task<ApiResult> AddTaskScheduleAsync(SysTasksQz tasksQz);

		Task<ApiResult> PauseTaskScheduleAsync(SysTasksQz tasksQz);

		Task<ApiResult> ResumeTaskScheduleAsync(SysTasksQz tasksQz);

		Task<ApiResult> DeleteTaskScheduleAsync(SysTasksQz tasksQz);

		Task<ApiResult> RunTaskScheduleAsync(SysTasksQz tasksQz);

		Task<ApiResult> UpdateTaskScheduleAsync(SysTasksQz tasksQz);
	}
}

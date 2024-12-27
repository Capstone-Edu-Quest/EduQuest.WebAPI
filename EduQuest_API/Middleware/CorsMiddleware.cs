namespace EduQuest_API.Middleware
{
	public class CorsMiddleware
	{
		private readonly RequestDelegate _next;

		public CorsMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
			context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
			context.Response.Headers.Add("Access-Control-Allow-Headers", "*");

			// Handle pre-flight requests
			if (context.Request.Method == "OPTIONS")
			{
				context.Response.StatusCode = 200;
				await context.Response.CompleteAsync();
				return;
			}

			// Call the next middleware in the pipeline
			await _next(context);
		}
	}
}

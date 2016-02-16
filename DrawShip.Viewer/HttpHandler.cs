﻿using Microsoft.Owin;
using System.Net;
using DrawShip.Common;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;

namespace DrawShip.Viewer
{
	public class HttpHandler
	{
		private readonly FileSystemFactory _fileSystemFactory;
		private readonly HostingContext _hostingContext;
		private readonly IRenderer _htmlRenderer;
		private readonly IRenderer _imageRenderer;

		public HttpHandler(
			HostingContext hostingContext,
			FileSystemFactory fileSystemFactory,
			IRenderer htmlRenderer,
			IRenderer imageRenderer)
		{
			_hostingContext = hostingContext;
			_htmlRenderer = htmlRenderer;
			_imageRenderer = imageRenderer;
			_fileSystemFactory = fileSystemFactory;
		}

		public async Task Handle(IOwinContext context)
		{
			try
			{
				var fileNameAndDirectoryKey = context.Request.Path.Value;
				var match = Regex.Match(fileNameAndDirectoryKey, @"^\/(?<directoryKey>.+?)\/(?<fileName>.+?)$");
				if (!match.Success)
				{
					await context.Respond(HttpStatusCode.BadRequest, "Invalid request - directoryKey and fileName cannot be extracted");
					return;
				}

				var fileName = match.Groups["fileName"].Value;
				var directoryKey = match.Groups["directoryKey"].Value;
				var directory = _hostingContext.GetDirectory(directoryKey);

				if (directory == null)
				{
					await context.Respond(HttpStatusCode.NotFound, "Directory not known - " + directoryKey);
					return;
				}

				var version = _GetVersion(context.Request.QueryString);
				var command = new ShowDiagramStructure
				{
					FileName = fileName,
					Directory = directory,
					Version = version
				};

				var drawing =  command.GetDrawing(fileName);
				if (!File.Exists(Path.Combine(drawing.FilePath, drawing.FileName)))
				{
					await context.Respond(HttpStatusCode.NotFound, "Drawing not : " + drawing.FileName);
					return;
				}

				var viewModel = new DrawingViewModel(
					drawing,
					_fileSystemFactory.GetFileSystem(context),
					version);
				var renderer = _GetRenderer(context.Request);
				renderer.RenderDrawing(new OwinResponseStream(context.Response), viewModel);
			}
			catch (Exception exc)
			{
				await context.Respond(HttpStatusCode.InternalServerError, exc.ToString());
			}
		}

		private IRenderer _GetRenderer(IOwinRequest request)
		{
			var queryString = HttpUtility.ParseQueryString(request.QueryString.Value);
			var formatString = queryString["f"];
			DiagramFormat format;
			if (!Enum.TryParse(formatString, true, out format))
				format = DiagramFormat.Html;

			switch (format)
			{
				case DiagramFormat.Image:
					return _imageRenderer;
				default:
					return _htmlRenderer;
			}
		}

		private static string _GetVersion(QueryString owinQueryString)
		{
			var queryString = HttpUtility.ParseQueryString(owinQueryString.Value);
			return queryString["v"];
		}
	}
}
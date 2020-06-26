using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	public sealed class WebService
	{
		private static WebService instance = new WebService();
		Appli appli = new Appli();
		Processing processing = new Processing();

		public WebService()
		{
		}

		public WebService Instance
		{
			get => instance;
		}

		public void LaunchAppli()
		{
			appli.Show();
			
		}
		
		public void LaunchProcessing()
        {
			processing.Show();
        }
	}
}

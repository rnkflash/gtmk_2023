using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Content.Scripts
{
	public class SceneController : Singleton<SceneController>
	{

		public void LoadGame()
		{
			Load("game");
		}
		
		public async void Load(string scene)
		{
			await LoadSceneAsync(scene, LoadSceneMode.Single);
		}

		private async Task LoadSceneAsync(string sceneName, LoadSceneMode mode)
		{
			var loadOp = SceneManager.LoadSceneAsync(sceneName, mode);

			while (!loadOp.isDone)
			{
				await Task.Delay(60);
			}
		}

		public void Restart()
		{
			var currentLevel = SceneManager.GetActiveScene().name;
			Load(currentLevel);
		}
	}
}
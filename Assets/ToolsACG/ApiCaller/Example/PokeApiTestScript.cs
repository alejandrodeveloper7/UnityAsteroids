using System.Threading.Tasks;
using ToolsACG.ApiCaller.PokeApi;
using UnityEngine;

public class PokeApiTestScript : MonoBehaviour
{
    #region Monobehaviour

    private void Start()
    {
        TestPokeApiService();
    }

    #endregion

    #region Initialization

    private void TestPokeApiService()
    {
        _ = TestService();
        //TestEnqueueService();
        //TestGeneralEnqueueService();
    }

    #endregion


    #region Funcionality


    private async Task TestService()
    {
        //This call will print true in console, call the Api and after receive the response, if all goes well, will print false in console

        Debug.Log("PikachuData is null: "+ (PokeApiService.Instance.PikachuData == null).ToString());
        await PokeApiService.Instance.GetPikachuData();
        Debug.Log("PikachuData is null: "+ (PokeApiService.Instance.PikachuData == null).ToString());
    }

    private void TestEnqueueService()
    {
        //This call will call to 5 Apis in order, without overlap any call

        _ = PokeApiService.Instance.EnqueueGetPikachuData();
        _ = PokeApiService.Instance.EnqueueGetCharmanderData();
        _ = PokeApiService.Instance.EnqueueGetPikachuData();
        _ = PokeApiService.Instance.EnqueueGetCharmanderData();
        _ = PokeApiService.Instance.EnqueueGetPikachuData();
    }

    private void TestGeneralEnqueueService()
    {
        //This call will call to 5 Apis in order, without overlap any call, and could be use with apis in diferent ApiCallerScripts

        _ = PokeApiService.Instance.GeneralEnqueueGetPikachuData();
        _ = PokeApiService.Instance.GeneralEnqueueGetCharmanderData();
        _ = PokeApiService.Instance.GeneralEnqueueGetPikachuData();
        _ = PokeApiService.Instance.GeneralEnqueueGetCharmanderData();
        _ = PokeApiService.Instance.GeneralEnqueueGetPikachuData();
    }

    #endregion
}

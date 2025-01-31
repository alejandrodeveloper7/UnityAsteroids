
namespace ToolsACG.Scenes.PlayerHealth
{
    public interface IPlayerHealthView
    {
        // TODO: declare here view methods to call them from controller context
    }

    public class PlayerHealthView : ModuleView, IPlayerHealthView
    {
        #region Fields        
        #endregion

        #region Protected Methods     

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        #region View Methods
        // TODO: define here view methods to call them from controller context
        #endregion

        #region Private Methods
        // TODO: define here methods called from view methods
        #endregion
    }
}
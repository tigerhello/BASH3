
using System.Windows.Controls;

namespace Demo.ui.view.page
{
    public class WorkspacePage : UserControl
    {
        public event MainFrame.OnExitWorkspace ExitWorkspaceEvent;

        public void OnExitWorkspace() {
            if (ExitWorkspaceEvent != null) {
                ExitWorkspaceEvent();
            }
            
        }

        public virtual Panel GetPageContent() {
            return null;
        }

        public virtual void LoadPage(int tubeIdx, int fromWorkspace, int toWorkspace) { }

        public virtual void UnloadPage(int tubeIdx, int workspaceIdx) { }
    }
}

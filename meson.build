project(
    'WinForMono',
    'cs'
)

CSHARP_ARGS = ['/r:System.Windows.Forms.dll', '/r:System.Drawing.dll']

WinForMono_src = files('IMessageBoxHandler.cs', 'UIAnchor.cs', 'UIButton.cs', 'UIBuilder.cs', 'UIComboBox.cs', 'UICheckbox.cs', 'UIElement.cs', 'UIPanel.cs', 'UITabs.cs', 'UITextBox.cs', 'UILabel.cs', 'UIPosition.cs', 'UIWindow.cs', 'Vector2.cs', 'WinformWrapper.cs')

WinForMono_dll = shared_library(
    'WinForMono',
    WinForMono_src,
    cs_args: CSHARP_ARGS
)

WinForMono_dep = declare_dependency(
    link_with: WinForMono_dll
)

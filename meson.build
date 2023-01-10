project(
    'WinForMono',
    'cs'
)

add_global_arguments(['/r:System.Windows.Forms.dll', '/r:System.Drawing.dll'], language: ['cs'])

WinForMono_src = files('UIAnchor.cs', 'UIButton.cs', 'UIBuilder.cs', 'UICheckbox.cs', 'UIElement.cs', 'UIPanel.cs', 'UITabs.cs', 'UITextBox.cs', 'UILabel.cs', 'UIPosition.cs', 'UIWindow.cs', 'Vector2.cs', 'WinformWrapper.cs')

WinForMono_dll = shared_library(
    'WinForMono',
    WinForMono_src,
)

WinForMono_dep = declare_dependency(
    link_with: WinForMono_dll
)

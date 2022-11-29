project('WinforMono', ['cs'])

add_global_arguments(['/r:System.Windows.Forms.dll', '/r:System.Drawing.dll'], language: ['cs'])

winformono_src = []
subdir('src')

winformono_dep = declare_dependency(shared_library(
    'WinforMono',
    winformono_src
))
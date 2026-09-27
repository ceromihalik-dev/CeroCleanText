param([string]$OutputPath)

Add-Type -AssemblyName System.Drawing

$size = 256
$bmp = New-Object System.Drawing.Bitmap $size, $size
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.Clear([System.Drawing.Color]::Transparent)

$blue = [System.Drawing.Color]::FromArgb(0, 120, 235)
$dark = [System.Drawing.Color]::FromArgb(35, 47, 62)
$white = [System.Drawing.Color]::White

# C-shaped blue brand ring.
$pen = New-Object System.Drawing.Pen $blue, 38
$pen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
$pen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
$g.DrawArc($pen, 28, 28, 190, 190, 45, 270)

# Three clean text lines.
$linePen = New-Object System.Drawing.Pen $dark, 14
$linePen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
$linePen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
$g.DrawLine($linePen, 78, 88, 151, 88)
$g.DrawLine($linePen, 78, 119, 143, 119)
$g.DrawLine($linePen, 78, 150, 126, 150)

# Broom handle and head.
$broomPen = New-Object System.Drawing.Pen $dark, 15
$broomPen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
$broomPen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
$g.DrawLine($broomPen, 173, 105, 142, 174)
$brush = @(
    (New-Object System.Drawing.Point 126,166),
    (New-Object System.Drawing.Point 154,178),
    (New-Object System.Drawing.Point 178,194),
    (New-Object System.Drawing.Point 154,218),
    (New-Object System.Drawing.Point 113,188)
)
$g.FillPolygon((New-Object System.Drawing.SolidBrush $dark), $brush)

$pngStream = New-Object System.IO.MemoryStream
$bmp.Save($pngStream, [System.Drawing.Imaging.ImageFormat]::Png)
$png = $pngStream.ToArray()

# ICO container with one 256x256 PNG image (0 means 256 in ICO directory).
$dir = New-Object System.IO.MemoryStream
$bw = New-Object System.IO.BinaryWriter $dir
$bw.Write([UInt16]0)
$bw.Write([UInt16]1)
$bw.Write([UInt16]1)
$bw.Write([Byte]0)
$bw.Write([Byte]0)
$bw.Write([Byte]0)
$bw.Write([Byte]0)
$bw.Write([UInt16]1)
$bw.Write([UInt16]32)
$bw.Write([UInt32]$png.Length)
$bw.Write([UInt32]22)
$bw.Write($png)
$bw.Flush()

$folder = Split-Path -Parent $OutputPath
if ($folder) { New-Item -ItemType Directory -Force -Path $folder | Out-Null }
[System.IO.File]::WriteAllBytes($OutputPath, $dir.ToArray())

$bw.Dispose()
$dir.Dispose()
$pngStream.Dispose()
$g.Dispose()
$bmp.Dispose()

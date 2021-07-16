# NetCore-DICOM-Helper

## Features
- Convert DCM file to JPG image

## Library
- [Fellow Oak DICOM](https://github.com/fo-dicom/fo-dicom) install nuget package
	- fo-dicom.NetCore `Install-Package fo-dicom.NetCore -Version 4.0.7`
	- fo-dicom.Drawing `Install-Package fo-dicom.Drawing -Version 4.0.7`

## Note
- resolve `image.RenderImage().AsBitmap();`  or `image.RenderImage().AsSharedBitmap();`
	- Exception **'Cannot cast to 'Bitmap'; type must be assignable from 'Byte[]'** Resolved by calling the following code on top:
`ImageManager.SetImplementation(new WinFormsImageManager());`
ref : [Cannot render a DICOM file to System.Drawing.Bitmap under .NET Core #877](https://github.com/fo-dicom/fo-dicom/issues/877)
	-	Exception **'The type initializer for  'Gdip' threw an exception'** (MacOS, not sure on Ubuntu docker) resolve by `brew install mono-libgdiplus`
	ref : [[Asp.net core] macOS Gdip error](https://www.programmersought.com/article/15514728099/)

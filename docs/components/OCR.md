# OCR

Optical Character Recognition (OCR) is one of the stretch-goals of this engagement. Because of time constraints, we didn't add this to the application, however some preparation work was done to make a future implementation easier.

## ComputerVisionHelper class

OCR can be implemented using Azure Cognitive Services - Computer Vision, which is part of the Azure Cognitive service already used for Speech in the HoloApp. A helper class called `ComputerVisionHelper` (in _CoreProject\Scripts\Helpers) was implemented with just one method `OCRProvidedImageAsync()` that takes the `byte[]` of images and returns a string of text with all found words.

## Suggested implementation

What could be done to include this in the HoloApp, is to add a **scan text** button to the **Advanced** settings of the **CameraControl** prefab. This button could be a toggle to use of OCR for images. If OCR is on, after taking a picture, the helper method can be used to obtain all text in the image and show that in the result pane in the **SaveUI** of the **CameraControls** prefab. This provides the user to see the result of the OCR and determine to save it or to retake the picture for a better OCR result.

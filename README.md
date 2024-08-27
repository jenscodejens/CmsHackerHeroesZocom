Redesigned Db to support 3 initial types of ContentTypes (string, arrays, images).

ContentType class is no longer an Enum. 

ContentType class is now abstract with 3 derived classes: ImageData, StringData, ArrayData.
It is now a TPH inheritance where'ContentTypeDisriminator' act as the discriminator.

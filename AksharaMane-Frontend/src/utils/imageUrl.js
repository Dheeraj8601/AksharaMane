import { appConfig } from "../config/appConfig";

const fallbackImage =
  "https://placehold.co/300x430?text=Book+Cover";

export function getImageUrl(imageUrl) {
  if (!imageUrl) {
    return fallbackImage;
  }

  if (
    imageUrl.startsWith("http://") ||
    imageUrl.startsWith("https://") ||
    imageUrl.startsWith("data:")
  ) {
    return imageUrl;
  }

  const normalizedPath = imageUrl.startsWith("/")
    ? imageUrl
    : `/${imageUrl}`;

  return `${appConfig.fileBaseUrl}${normalizedPath}`;
}

export function setFallbackImage(event) {
  event.currentTarget.onerror = null;
  event.currentTarget.src = fallbackImage;
}
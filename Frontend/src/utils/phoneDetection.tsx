export function findPhoneNumbers(message: string) {
  return message.matchAll(/\b\d{10}\b/g);
}

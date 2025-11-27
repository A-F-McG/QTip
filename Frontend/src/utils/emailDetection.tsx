export function findEmails(message: string) {
  const wordBoundary = /\b/;
  const local = /[A-Za-z\d\.\-\_]+/; // allow letters, numbers, periods, hyphens and underscores
  const domainName = /[A-Za-z\d]+/; // allow letters and numbers
  const topLevelDomain = /[A-Za-z]+/;

  const emailRegex = new RegExp(
    `${wordBoundary.source}${local.source}@${domainName.source}\\.${topLevelDomain.source}${wordBoundary.source}`,
    "g"
  );

  return message.matchAll(emailRegex);
}

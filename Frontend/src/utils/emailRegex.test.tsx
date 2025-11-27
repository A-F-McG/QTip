import { findEmails } from "./emailDetection";

describe("Email detection", () => {
  it("should find 0 emails string is empty", () => {
    expect(Array.from(findEmails(""))).toHaveLength(0);
  });

  it("should find 0 emails if there's no period", () => {
    expect(Array.from(findEmails("test@emailcom"))).toHaveLength(0);
  });

  it("should find 1 email if there's an email", () => {
    const emails = Array.from(
      findEmails("here is an email test@email.com more filler words")
    );
    expect(emails).toHaveLength(1);
    expect(emails[0]).toBe("test@email.com");
  });

  it("should find an email even if it's surrounded by punctuation", () => {
    expect(Array.from(findEmails("!test@email.com!"))).toHaveLength(1);
  });

  it("should find multiple emails", () => {
    expect(
      Array.from(findEmails("!test@email.com! test@email.com"))
    ).toHaveLength(2);
  });

  it("should find 1 email if 2 emails are combined (a bug really, but this an edge case not delved into in this project)", () => {
    expect(Array.from(findEmails("test@email.comtest@email.com"))).toHaveLength(
      1
    );
  });
});

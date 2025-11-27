import "./App.scss";
import { useState, ReactElement, useEffect, useCallback } from "react";
import { findEmails } from "./utils/emailDetection";
import { wrapTextWithTooltips } from "./components/tooltip";
import { getEmailCount, sendSubmission } from "./services/piiService";

export default function Form() {
  const [plainText, setPlainText] = useState<string>("");
  const [annotatedText, setAnnotatedText] = useState<ReactElement>();
  const [emailCount, setEmailCount] = useState<number>(0);
  const [fetchingEmailCount, setFetchingEmailCount] = useState<boolean>(false);

  const fetchEmailCount = useCallback(async () => {
    setFetchingEmailCount(true);
    try {
      setEmailCount(await getEmailCount());
    } catch (err) {
    } finally {
      setFetchingEmailCount(false);
    }
  }, []);

  useEffect(() => {
    fetchEmailCount();
  }, []);

  useEffect(() => {
    const emails = findEmails(plainText);
    setAnnotatedText(
      wrapTextWithTooltips(plainText, emails, "PII - email address")
    );
  }, [plainText]);

  return (
    <div className="page">
      <h1>QTip, the data spellchecker</h1>

      <p aria-live="polite" className="stats">
        Total distinct PII emails submitted:{" "}
        {fetchingEmailCount ? "...running the numbers..." : emailCount}
      </p>

      <form
        className="form"
        onSubmit={(e) => {
          e.preventDefault();
          sendSubmission(plainText)
            .then(fetchEmailCount)
            .catch((err) => console.error(err));
        }}
      >
        <label htmlFor="input">Write a message!</label>
        <textarea id="input" onChange={(e) => setPlainText(e.target.value)} />

        <button
          type="submit"
          className="button"
          disabled={plainText.length === 0}
        >
          Submit
        </button>
      </form>

      <h2>Your message with any email addresses underlined:</h2>
      <p>{annotatedText}</p>
    </div>
  );
}

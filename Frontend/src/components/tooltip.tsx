import { ReactElement } from "react";
import "./tooltip.scss";

function Tooltip({
  text,
  tooltipText,
}: {
  text: string;
  tooltipText: string;
}) {
  return (
    <span tabIndex={0} className="tooltip piiDetectionWarning">
      {text}
      <span>{tooltipText}</span>
    </span>
  );
}

export function wrapTextWithTooltips(message: string, itemsToWrap: RegExpStringIterator<RegExpExecArray>, tooltipText: string): ReactElement {
    const annotatedText = [];
    let currentIndex = 0;

    for (const item of itemsToWrap) {
      const text = item[0];

      annotatedText.push(message.slice(currentIndex, item.index));
      annotatedText.push(
        <Tooltip text={text} tooltipText={tooltipText} />
      );

      currentIndex = item.index + text.length;
    }

    annotatedText.push(message.slice(currentIndex, message.length));
    return <>{annotatedText}</>;
  }

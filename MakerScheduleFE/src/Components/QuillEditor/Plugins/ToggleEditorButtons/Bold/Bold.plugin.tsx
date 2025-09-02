import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { ToggleButton } from "@mui/material";
import { FORMAT_TEXT_COMMAND } from "lexical";
import { useState } from "react";
import FormatBoldIcon from "@mui/icons-material/FormatBold";

const BoldPlugin = () => {
  const [isBold, setIsBold] = useState(false);
  const [editor] = useLexicalComposerContext();

  const toggleBold = () => {
    editor.dispatchCommand(FORMAT_TEXT_COMMAND, "bold");

    setIsBold((prev) => !prev);
  };

  return (
    <ToggleButton
      value="bold"
      aria-label="bold"
      onClick={toggleBold}
      selected={isBold}
    >
      <FormatBoldIcon />
    </ToggleButton>
  );
};

export { BoldPlugin };

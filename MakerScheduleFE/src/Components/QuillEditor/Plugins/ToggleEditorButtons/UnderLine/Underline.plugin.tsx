import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { ToggleButton } from "@mui/material";
import { FORMAT_TEXT_COMMAND } from "lexical";
import { useState } from "react";
import FormatUnderlinedIcon from "@mui/icons-material/FormatUnderlined";

const UnderlinePlugin = () => {
  const [isUnderline, setIsUnderline] = useState(false);
  const [editor] = useLexicalComposerContext();

  const toggleUnderline = () => {
    editor.dispatchCommand(FORMAT_TEXT_COMMAND, "underline");

    setIsUnderline((prev) => !prev);
  };

  return (
    <ToggleButton
      value="underline"
      aria-label="underline"
      onClick={toggleUnderline}
      selected={isUnderline}
    >
      <FormatUnderlinedIcon />
    </ToggleButton>
  );
};

export { UnderlinePlugin };

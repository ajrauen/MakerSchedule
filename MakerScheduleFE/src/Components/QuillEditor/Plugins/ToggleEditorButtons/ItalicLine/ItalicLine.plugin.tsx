import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { ToggleButton } from "@mui/material";
import { FORMAT_TEXT_COMMAND } from "lexical";
import { useState } from "react";
import FormatItalicIcon from "@mui/icons-material/FormatItalic";

const ItalicPlugin = () => {
  const [isItalic, setIsItalic] = useState(false);
  const [editor] = useLexicalComposerContext();

  const toggleItalic = () => {
    editor.dispatchCommand(FORMAT_TEXT_COMMAND, "italic");

    setIsItalic((prev) => !prev);
  };

  return (
    <ToggleButton
      value="italic"
      aria-label="italic"
      onClick={toggleItalic}
      selected={isItalic}
    >
      <FormatItalicIcon />
    </ToggleButton>
  );
};

export { ItalicPlugin };

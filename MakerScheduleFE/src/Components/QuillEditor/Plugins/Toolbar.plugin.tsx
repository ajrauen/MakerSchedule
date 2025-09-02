import { BoldPlugin } from "@ms/Components/QuillEditor/Plugins/ToggleEditorButtons/Bold/Bold.plugin";
import { ItalicPlugin } from "@ms/Components/QuillEditor/Plugins/ToggleEditorButtons/ItalicLine/ItalicLine.plugin";
import { UnderlinePlugin } from "@ms/Components/QuillEditor/Plugins/ToggleEditorButtons/UnderLine/Underline.plugin";

const ToolbarPlugin = () => {
  return (
    <div className="toolbar">
      <BoldPlugin />
      <UnderlinePlugin />
      <ItalicPlugin />
    </div>
  );
};

export { ToolbarPlugin };

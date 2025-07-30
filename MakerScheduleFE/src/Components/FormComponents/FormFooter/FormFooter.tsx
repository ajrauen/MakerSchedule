import { Button } from "@mui/material";

interface FormFooterProps {
  onCancel: () => void;
  onSubmit?: () => void;
  areActionsDisabled: boolean;
  isLoading: boolean;
  cancelButtonText?: string;
  saveButtonText?: string;
}

const FormFooter = ({
  onCancel,
  onSubmit,
  areActionsDisabled,
  isLoading,
  cancelButtonText = "Cancel",
  saveButtonText = "Save",
}: FormFooterProps) => {
  return (
    <div className="pt-4 ml-auto gap-3 flex">
      <Button type="button" onClick={onCancel} disabled={areActionsDisabled}>
        {cancelButtonText}
      </Button>
      <Button
        type="submit"
        variant="contained"
        disabled={areActionsDisabled}
        loading={isLoading}
        onClick={onSubmit}
      >
        {saveButtonText}
      </Button>
    </div>
  );
};

export { FormFooter };

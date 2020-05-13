import { Component, FunctionComponent } from "react";
import { TextInput } from "../common/components/form";
import { TextEditor } from "../common/components/form";
import { FieldTypes } from '../contenttypes/models';

export const editorMap: Record<string, Component|FunctionComponent> = {
  [FieldTypes.ShortText]: TextInput,
  [FieldTypes.LongText]: TextEditor,
  [FieldTypes.Number]: TextInput,
  [FieldTypes.DateTime]: TextInput
};
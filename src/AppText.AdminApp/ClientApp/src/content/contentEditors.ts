import { Component, FunctionComponent } from "react";
import { TextInput, NumberInput } from "../common/components/form";
import { TextEditor } from "../common/components/form";
import { DateTimeInput } from '../common/components/form';
import { FieldTypes } from '../contenttypes/models';

export const editorMap: Record<string, Component|FunctionComponent> = {
  [FieldTypes.ShortText]: TextInput,
  [FieldTypes.LongText]: TextEditor,
  [FieldTypes.Number]: NumberInput,
  [FieldTypes.DateTime]: DateTimeInput
};
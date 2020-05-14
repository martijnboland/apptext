import React from 'react';
import { FieldProps } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';
import { TextInput } from './TextInput';

interface INumberInputProps extends ICustomFieldProps{
}

export const NumberInput: React.FunctionComponent<INumberInputProps & FieldProps> = (props) => {
  return <TextInput type="number" {...props} />;
};
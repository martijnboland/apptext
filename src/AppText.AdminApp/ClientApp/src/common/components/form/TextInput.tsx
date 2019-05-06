import React from 'react';
import classNames from 'classnames';
import { FieldProps, getIn } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';

interface TextInputProps extends ICustomFieldProps {
  type?: string,
}

export const TextInput: React.FunctionComponent<FieldProps & TextInputProps> = ({
  type,
  label,
  className,
  field, // { name, value, onChange, onBlur }
  form: { touched, errors }, // also values, setXXXX, handleXXXX, dirty, isValid, status, etc.
  ...rest
}) => {
  const inputType = type || 'text';
  const cssClass = className || 'form-group';
  const error = getIn(errors, field.name);
  const touch = getIn(touched, field.name);
  return (
    <div className={cssClass}>
      <label>{label}</label>
      <input type={inputType} {...field} {...rest} className={classNames('form-control', { 'is-invalid': error })} />
      {error && touch && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};
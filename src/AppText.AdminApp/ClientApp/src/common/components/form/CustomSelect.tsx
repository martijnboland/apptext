import { FieldProps, getIn } from 'formik';
import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import { OptionsType, ValueType } from 'react-select/src/types';
import { ICustomFieldProps } from './ICustomFieldProps';

interface Option {
  label: string;
  value: string;
}

interface CustomSelectProps extends FieldProps, ICustomFieldProps {
  options: OptionsType<Option>;
  isMulti?: boolean;
}

export const CustomSelect: React.FunctionComponent<CustomSelectProps> = ({ label, className, field, form, options, isMulti = false }) => {
  
  const [localOptions, setLocalOptions] = useState<OptionsType<Option>>(undefined);

  useEffect(() => {
    if (localOptions && localOptions.length > 0 && options.length === 0) {
      // options are cleared, also clear form value
      setLocalOptions(options);
      form.setFieldValue(field.name, null);
      console.log('Options cleared, clear also form value.')
    }
    if (localOptions === undefined || options.length > localOptions.length) {
      setLocalOptions(options);
    }  
  });

  const onChange = (option: ValueType<Option | Option[]>) => {
    console.log(`Value of ${field.name} changed =>`, option)
    form.setFieldValue(
      field.name,
      option !== null
        ? isMulti
          ? (option as Option[]).map((item: Option) => item.value)
          : (option as Option).value
        : []
    );
  };

  const getValue = () => {
    if (options) {
      return isMulti
        ? options.filter(option => field.value.indexOf(option.value) >= 0)
        : options.find(option => option.value === field.value);
    } else {
      return isMulti ? [] : '' as any;
    }
  };

  const cssClass = className || 'form-group';
  const error = getIn(form.errors, field.name);
  const touch = getIn(form.touched, field.name);

  return (
    <div className={cssClass}>
      <label htmlFor={field.name}>{label}</label>
      <Select name={field.name} value={getValue()} onChange={onChange} options={options} isMulti={isMulti} />
      {error && touch && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};
import React from 'react';
import ReactMde from 'react-mde';
import classNames from 'classnames';
import Showdown from 'showdown';
import { FieldProps, getIn } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';

import "react-mde/lib/styles/css/react-mde-all.css";

interface TextEditorProps extends ICustomFieldProps {
}

export const TextEditor: React.FC<TextEditorProps & FieldProps> = ({ 
  label,
  className,
  field,
  form,
  ...rest
}) => {

  const cssClass = className || 'form-group';
  const error = getIn(form.errors, field.name);
  const touch = getIn(form.touched, field.name);

  const converter = new Showdown.Converter({
    tables: true,
    simplifiedAutoLink: true,
    strikethrough: true,
    tasklists: true
  });
  
  const onChange = (value) => {
    form.setFieldValue(field.name, value);
  }

  const [selectedTab, setSelectedTab] = React.useState<"write" | "preview">("write");

  return (
    <div className={cssClass}>
      <label htmlFor={field.name}>{label}</label>
      <ReactMde
        value={field.value}
        onChange={onChange}
        classes={{ reactMde: classNames({ 'is-invalid': error })}}
        selectedTab={selectedTab}
        onTabChange={setSelectedTab}
        generateMarkdownPreview={markdown =>
          Promise.resolve(converter.makeHtml(markdown))
        }
        {...rest}
      />
      {error && <div className="invalid-feedback">{error}</div>}
    </div>
  );
}
import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'camelSpace',
  standalone: true,
})
export class CamelSpacePipe implements PipeTransform {
  transform(value: string | undefined): string {
    return this.toCamelSpace(value);
  }

  toCamelSpace(value: string | undefined): string {
    if (!value) {
      return '';
    }

    let result = value[0];

    for (let i = 1; i < value.length; i++) {
      const code = value.charCodeAt(i);

      if (code >= 65 && code <= 90) {
        result += ' ';
      }

      result += value[i];
    }

    return result;
  }
}
